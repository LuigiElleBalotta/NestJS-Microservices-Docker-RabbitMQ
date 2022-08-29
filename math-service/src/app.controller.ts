import { Controller, Get } from '@nestjs/common';
import { AppService } from './app.service';
import { MessagePattern } from "@nestjs/microservices";

@Controller()
export class AppController {
    constructor(private readonly appService: AppService) {}

    @MessagePattern({ cmd: 'sum' })
    async accumulate( data: number[]): Promise<number> {
        return (data || []).reduce((a, b) => a + b);
    }

    @MessagePattern({ cmd: 'multiply' })
    async multiply( data: number[]): Promise<number> {
        return (data || []).reduce((a, b) => a * b);
    }
}
