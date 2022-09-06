export default () => ({
    application: {
        port: parseInt(process.env.PORT, 10) || 3000,
        environment: process.env.NODE_ENV || 'local'
    },

    rabbitmq: {
        protocol: process.env.RMQ_PROTOCOL || 'amqp',
        host: process.env.RMQ_HOST || 'rabbitmq',
        port: parseInt(process.env.RMQ_PORT, 10) || 5672,
        useAuthentication: parseInt(process.env.RMQ_USE_AUTHENTICATION) === 1 || false,
        username: process.env.RMQ_USERNAME || 'bbsway',
        password: process.env.RMQ_PASSWORD || 'th&bbsw4y',
    },

    mathService: {
        queueName: process.env.MS_MATH_SERVICE_QUEUE || 'math_queue'
    },
    dotnetEchoService: {
        queueName: process.env.MS_DOTNET_ECHO_SERVICE_QUEUE || 'echo-service'
    }
})
