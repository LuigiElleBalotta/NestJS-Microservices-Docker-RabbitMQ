import { Module } from '@nestjs/common';
import { AppController } from './app.controller';
import { AppService } from './app.service';
import {ClientsModule, Transport} from "@nestjs/microservices";
import {Microservices} from "./shared";

@Module({
  imports: [
      ClientsModule.register([
          {
              name: Microservices.MATH_SERVICE,
              transport: Transport.RMQ,
              options: {
                urls: ['amqp://rabbitmq:5672'],
                queue: 'math_queue',
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
