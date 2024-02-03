import http from 'k6/http';
import { sleep } from 'k6';
import { uuidv4 } from 'https://jslib.k6.io/k6-utils/1.4.0/index.js';
import { Counter } from 'k6/metrics';

// A counter for 404 responses
let Counter404 = new Counter('http_404');

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '5m',  target: 500 }, // start picking up users, ramping up to 500 users over 5 minutes
        { duration: '10m',  target: 500 }, //stay at 500 users for 10 minutes
        { duration: '5m',  target: 0 }, // scale down. Recovery stage.
       ],
       thresholds: {
        http_req_duration: ['p(99)<150'], // 99% of requests must complete below 150ms},
       }
}

export default function () {
    const url = 'http://localhost:8000/match';
    const randomUUID = uuidv4();

    // generate random number between 500 and 2500
    const number = Math.floor(Math.random() * 2000) + 500;
    
    const data = {
        "playerId": randomUUID,
        "elo": number
    };

    const headers = { 'Content-Type': 'application/json' };

    const result = http.post(url, JSON.stringify(data),
    {
        headers: headers
    });

    // If the response is 404, increment the counter
    if (result.status === 404) {
        Counter404.add(1);
    }

    sleep(1);
}