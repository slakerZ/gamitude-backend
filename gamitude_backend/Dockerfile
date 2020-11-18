FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
RUN ls
COPY bin/Release/netcoreapp3.1/publish/ .
ENTRYPOINT ["dotnet", "gamitude_backend.dll"]