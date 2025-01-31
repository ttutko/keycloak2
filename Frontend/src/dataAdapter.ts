import { HttpClient } from 'aurelia-fetch-client';
import { autoinject } from 'aurelia-framework';

@autoinject
export class DataAdapter {
  private baseUrl = "https://web.dev.fa.com";
  private secureUrl = "/secure";
  private anonymousUrl = "/insecure";

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
