import {Body, Controller, Get, Inject, Post} from '@nestjs/common';
import { AppService } from './app.service';
import {Microservices} from "./shared";
import {ClientProxy} from "@nestjs/microservices";
import {Observable} from "rxjs";

@Controller()
export class AppController {
  constructor(
      @Inject(Microservices.MATH_SERVICE) private mathClient: ClientProxy
  ) {}


    @Post('accumulate')
    accumulate(@Body() data: number[]): Observable<number> {

        const pattern = { cmd: 'sum' };

        return this.mathClient.send<number>(pattern, data);
    }

    @Post('multiply')
    multiply( @Body() data: number[]): Observable<number> {

        const pattern = { cmd: 'multiply' };

        return this.mathClient.send<number>(pattern, data);
    }
}
