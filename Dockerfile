FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /app

# copy csproj and restore as distinct layers
# COPY *.sln .
COPY *.csproj ./Test/
WORKDIR /app/Test
RUN dotnet restore

# copy everything else and build app
WORKDIR /app
COPY . ./Test/
WORKDIR /app/Test
RUN dotnet publish -c Release -o out


FROM microsoft/dotnet:2.1-aspnetcore-runtime AS runtime
RUN ["apt-get","update"]
RUN ["apt-get","install","-y","apt-file"]
RUN ["apt-get","update"]
RUN ["apt-get","install","-y","vim"]
WORKDIR /app
COPY --from=build /app/Test/out ./
ENTRYPOINT ["dotnet", "Test.dll"]

EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV classname=$classname