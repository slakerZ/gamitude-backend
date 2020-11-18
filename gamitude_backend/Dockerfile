FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY bin/Release/netcoreapp3.1/publish/ .
RUN ls 
ENTRYPOINT ["dotnet", "gamitude_backend.dll"]