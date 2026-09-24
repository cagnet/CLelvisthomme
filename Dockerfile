# Étape 1 : build — compile l'API avec le SDK complet (non nécessaire à l'exécution).
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copie uniquement les .csproj d'abord pour tirer parti du cache Docker sur `dotnet restore`
# (les couches ne sont invalidées que si les dépendances changent, pas à chaque édition de code).
COPY src/CustomerOrders.Api/CustomerOrders.Api.csproj src/CustomerOrders.Api/
COPY src/CustomerOrders.Business/CustomerOrders.Business.csproj src/CustomerOrders.Business/
COPY src/CustomerOrders.Data/CustomerOrders.Data.csproj src/CustomerOrders.Data/
RUN dotnet restore src/CustomerOrders.Api/CustomerOrders.Api.csproj

COPY src/ src/
RUN dotnet publish src/CustomerOrders.Api/CustomerOrders.Api.csproj -c Release -o /app --no-restore

# Étape 2 : runtime — image ASP.NET minimale, sans SDK ni sources, pour une image finale plus légère et une surface d'attaque réduite.
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Utilisateur non-root : l'image de base fournit déjà cet utilisateur applicatif dédié.
USER app

COPY --from=build /app .

EXPOSE 8080
ENTRYPOINT ["dotnet", "CustomerOrders.Api.dll"]
