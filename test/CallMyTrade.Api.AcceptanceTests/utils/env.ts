const BEFORE_ALL_TIMEOUT = Number(process.env.HTTP_TIMEOUT) || 5000; // 5 sec
const HOST = process.env.HOST || 'http://localhost';

export { BEFORE_ALL_TIMEOUT, HOST };
