# Step 1: Use the official Node.js 20 image as the base image
FROM node:18.20.5 AS build
RUN corepack enable
# Step 2: Set the working directory in the container
WORKDIR /app

# Step 3: Copy package.json and package-lock.json (or yarn.lock)
#COPY package*.json ./

# Step 5: Copy the rest of the project files into the container
COPY . .

# Step 4: Install project dependencies
RUN yarn install


# Step 6: Build the Aurelia 2 project
RUN yarn build

# Step 7: Use Nginx to serve the application
FROM nginx:alpine

# Step 8: Copy the build artifacts from the previous stage
COPY --from=build /app/dist /usr/share/nginx/html

# Step 9: Expose port 80 to the outside once the container has launched
EXPOSE 80

# Step 10: Start Nginx and serve the application
CMD ["nginx", "-g", "daemon off;"]



#FROM node:18.20.5
#WORKDIR /src
#ADD . .
#RUN corepack enable
#RUN yarn install
#RUN yarn build
#ENTRYPOINT ["yarn", "start"]
