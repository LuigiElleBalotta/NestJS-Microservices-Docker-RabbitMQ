import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import { MicroserviceOptions, Transport } from "@nestjs/microservices";
import { Logger } from "@nestjs/common";

async function bootstrap() {
    const logger = new Logger("Math Service");

    const app = await NestFactory.createMicroservice<MicroserviceOptions>(
        AppModule,
        {
            transport: Transport.RMQ,
            options: {
                urls: [`${process.env.RMQ_PROTOCOL}://${ Number(process.env.RMQ_USE_AUTHENTICATION) === 1 ? `${process.env.RMQ_USERNAME}:${process.env.RMQ_PASSWORD}@` : '' }${process.env.RMQ_HOST}:${process.env.RMQ_PORT}`],
                queue: process.env.MS_MATH_SERVICE_QUEUE,
                queueOptions: {
                    durable: false
                }
            }
        }
    );

    logger.debug(`The environment is: ${process.env.NODE_ENV}`);

    await app.listen();
}
bootstrap();
