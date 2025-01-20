FROM node:18.20.5 AS ui-build
RUN corepack enable
# Step 2: Set the working directory in the container
WORKDIR /app
# Step 3: Copy package.json and package-lock.json (or yarn.lock)
#COPY package*.json ./
# Step 5: Copy the rest of the project files into the container
COPY Frontend/. .
# Step 4: Install project dependencies
RUN yarn install
# Step 6: Build the Aurelia 2 project
RUN yarn build

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 5000

ENV ASPNETCORE_URLS=http://+:5000

USER app
FROM --platform=$BUILDPLATFORM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["Backend/Backend.csproj", "Backend/"]
RUN dotnet restore "Backend/Backend.csproj"
COPY . .
WORKDIR "/src/Backend"
RUN dotnet build "Backend.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "Backend.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
ADD smooth.tnt.root.crt /usr/local/share/ca-certificates/
RUN update-ca-certificates
WORKDIR /app
COPY --from=publish /app/publish .
COPY --from=ui-build /app/dist ./wwwroot
ENTRYPOINT ["dotnet", "Backend.dll"]
