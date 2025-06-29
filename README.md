
# Generic App

## Introduction

The goal is to have a base code with best practices to use as an start point for web app in .net 9

## Start & Requirements

To start this solution on your local machine, you would need:
- docker-compose
- .Net framework 9

Once you have the dependencies, you should do:
- open a terminal con the root of the project and then do `docker-compose up` to create the mongodb and mongo express.
  - mongo express will run in port 8081 check the docker-compose.yml file to get the user and password.
- open your .net IDE and run the application, swagger should rise in your browser. 


## Components

Following the best practices we have created a framework to be used on application to allow the developers to just focus on business.

### Framework

We have implemented a few layers:
- CQRS
- Security
- Repository

