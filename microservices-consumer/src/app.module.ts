import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import {ClientsModule, Transport} from "@nestjs/microservices";
import {Microservices} from "./shared";
import {ConfigModule} from "@nestjs/config";
import configuration from "./Core/Configuration/configuration";

@Module({
  imports: [
      ConfigModule.forRoot({
          isGlobal: true,
          envFilePath: `.env.${process.env.NODE_ENV}`,
          load: [configuration],
          cache: true
      }),

      ClientsModule.register([
          {
              name: Microservices.MATH_SERVICE,
              transport: Transport.RMQ,
              options: {
                urls: [`${process.env.RMQ_PROTOCOL}://${ Number(process.env.RMQ_USE_AUTHENTICATION) === 1 ? `${process.env.RMQ_USERNAME}:${process.env.RMQ_PASSWORD}@` : '' }${process.env.RMQ_HOST}:${process.env.RMQ_PORT}`],
                queue: process.env.MS_MATH_SERVICE_QUEUE,
                queueOptions: {
                    durable: false,
                }
              }
          },
          {
              name: Microservices.DOTNET_ECHO_SERVICE,
              transport: Transport.RMQ,
              options: {
                  urls: [`${process.env.RMQ_PROTOCOL}://${ Number(process.env.RMQ_USE_AUTHENTICATION) === 1 ? `${process.env.RMQ_USERNAME}:${process.env.RMQ_PASSWORD}@` : '' }${process.env.RMQ_HOST}:${process.env.RMQ_PORT}`],
                  queue: process.env.MS_DOTNET_ECHO_SERVICE_QUEUE,
                  queueOptions: {
                      durable: false,
                  }
              }
          }
      ])
  ],
  controllers: [AppController],
  providers: [AppService],
})
export class AppModule {}
