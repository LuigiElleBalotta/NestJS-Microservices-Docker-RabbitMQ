import {Body, Controller, Get, Inject, Post} from '@nestjs/common';
import { AppService } from './app.service';
import {Microservices} from "./shared";
import {ClientProxy} from "@nestjs/microservices";
import {Observable, of} from "rxjs";

@Controller()
export class AppController {
  constructor(
      @Inject(Microservices.MATH_SERVICE) private mathClient: ClientProxy,
      @Inject(Microservices.DOTNET_ECHO_SERVICE) private echoClient: ClientProxy
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

    @Post('echo')
    echo(@Body() data: { text: string }): Observable<string> {

        const pattern = { cmd: 'echo' };

        try {
            return this.echoClient.send<string>(pattern, data);
        }
        catch( error ) {
            console.log('error', error);
            return of(error);
        }

    }
}
