import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';
import {Logger} from "@nestjs/common";

async function bootstrap() {
    const logger = new Logger('MS Consumer');
    const app = await NestFactory.create(AppModule);

    logger.debug(`The environment is: ${process.env.NODE_ENV}`);

    await app.listen(Number(process.env.PORT));
}
bootstrap();
