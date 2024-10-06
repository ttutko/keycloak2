import { HttpClient } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';

@autoinject
export class DataAdapter {
    private baseUrl = "https://localhost:5001";

    constructor(private httpClient: HttpClient) {

    }

    public async getUnsecuredPage() {
        let response = await this.httpClient.fetch(`${this.baseUrl}`, {
            method: "GET"
        });

        return await response.text();
    }

    public async getSecuredPage() {
        let response = await this.httpClient.fetch(`${this.baseUrl}/secure`, {
            method: "GET"
        });

        return await response.text();
    }
}