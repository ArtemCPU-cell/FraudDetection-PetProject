import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 10 },
        { duration: '30s', target: 50 },
        { duration: '30s', target: 100 },
        { duration: '10s', target: 0 },
    ],

    thresholds: {
        http_req_failed: ['rate<0.01'],
        http_req_duration: ['p(95)<1000'],
    },
};

export default function () {
    const payload = JSON.stringify({
        UserId: 'artem',
        Amount: 1000000,
        Currency: 'RU',
        DeviceId: 'androeed',
        Country: 'RU',
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const response = http.post(
        'https://localhost:7066/transaction/check',
        payload,
        params
    );

    check(response, {
        'status is 200': (r) => r.status === 200,
    });

    sleep(0.3);
}