import { HttpClient } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';

@autoinject
export class DataAdapter {
  private baseUrl = "http://localhost:5024";
  private secureUrl = "http://192.168.1.168:5141/weatherforecastsecure";
  private anonymousUrl = "http://192.168.1.168:5141/weatherforecast";

  constructor(private httpClient: HttpClient) {

  }

  public async getUnsecuredPage() {
    const response = await this.httpClient.fetch(`${this.anonymousUrl}`, {
      method: "GET"
    });

    return await response.text();
  }

  public async getSecuredPage() {
    const response = await this.httpClient.fetch(`${this.secureUrl}`, {
      method: "GET",
      credentials: "include"

    });

    return await response.text();
  }
}
