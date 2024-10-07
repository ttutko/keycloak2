import { HttpClient } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';

@autoinject
export class DataAdapter {
  private baseUrl = "http://localhost:5024";

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
      method: "GET",
      credentials: "include"

    });

    return await response.text();
  }
}
