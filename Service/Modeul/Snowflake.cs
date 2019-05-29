
using System;
using System.Threading;
using Test.Interface;

namespace Test.Service
{
    /// <summary>
    /// 动态生产有规律的ID
    /// </summary>
    public class Snowflake : ISnowflake
    {
        private static long machineId;
        private static long datacenterId = 0L;
        private static long sequence = 0L;
	    private static long twepoch = 687888001020L;
	    private static long machineIdBits = 5L;
        private static long datacenterIdBits = 5L;
        public static long maxMachineId = -1L ^ -1L << (int)machineIdBits;
        private static long maxDatacenterId = -1L ^ (-1L << (int)datacenterIdBits);
	    private static long sequenceBits = 12L;
	    private static long machineIdShift = sequenceBits;
        private static long datacenterIdShift = sequenceBits + machineIdBits;
        private static long timestampLeftShift = sequenceBits + machineIdBits + datacenterIdBits;
        public static long sequenceMask = -1L ^ -1L << (int)sequenceBits;
	    private static long lastTimestamp = -1L;
        private readonly SemaphoreSlim _connectionLock = new SemaphoreSlim(initialCount: 1, maxCount: 1);
        private static Snowflake snowflake;
        public static Snowflake Instance(long machineId)
        {
            if (snowflake == null)
                snowflake = new Snowflake();
            return snowflake;
        }

        public Snowflake() => Snowflakes(0L, -1);
        public Snowflake(long machineId) => Snowflakes(machineId, -1);
        public Snowflake(long machineId, long datacenterId) => Snowflakes(machineId, datacenterId);
        private void Snowflakes(long machineId, long datacenterId)
        {
            if (machineId >= 0)
            {
                if (machineId > maxMachineId)
                    throw new Exception("Incorrect Machine Id");
                Snowflake.machineId = machineId;
            }
            if (datacenterId >= 0)
            {
                if (datacenterId > maxDatacenterId)
                    throw new Exception("Incorrect Data Id");
                Snowflake.datacenterId = datacenterId;
            }
        }

        /// <summary>
        /// 生成当前时间戳
        /// </summary>
        /// <returns>毫秒</returns>
        private static long GetTimestamp() => (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds;

        /// <summary>
        /// 获取下一微秒时间戳
        /// </summary>
        /// <param name="lastTimestamp"></param>
        /// <returns></returns>
        private static long GetNextTimestamp(long lastTimestamp)
        {
            long timestamp = GetTimestamp();
            if (timestamp <= lastTimestamp)
                timestamp = GetTimestamp();
            return timestamp;
        }

        /// <summary>
        /// 获取长整形的ID
        /// </summary>
        /// <returns></returns>
        public long GetId()
        {
            _connectionLock.Wait();
            try
            {
                long timestamp = GetTimestamp();
                if (Snowflake.lastTimestamp == timestamp)
                {
                    sequence = (sequence + 1) & sequenceMask;
                    if (sequence == 0)
                        timestamp = GetNextTimestamp(Snowflake.lastTimestamp);
                }
                else
                    sequence = 0L;
                if(timestamp < lastTimestamp)
                    throw new Exception("Timestamp Value Early Then Last Time");
                Snowflake.lastTimestamp = timestamp;
                long Id = ((timestamp - twepoch) << (int)timestampLeftShift)
                    | (datacenterId << (int)datacenterIdShift)
                    | (machineId << (int)machineIdShift)
                    | sequence;
                return Id;
            }
            finally
            {
                _connectionLock.Release();
            }
        }
    }
}