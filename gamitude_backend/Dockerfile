FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS runtime
WORKDIR /app
COPY bin/Release/netcoreapp5.0/publish/ .
RUN ls 
ENTRYPOINT ["dotnet", "gamitude_backend.dll"]