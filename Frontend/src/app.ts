import { KeycloakService } from 'keycloackService';
import { autoinject } from 'aurelia-framework';
import { DataAdapter } from 'dataAdapter';

@autoinject
export class App {
  private lastWebRequest = "";
  constructor(private keyCloakService: KeycloakService, private dataAdapter: DataAdapter) {

  }

  async attached() {
    try {
      await this.keyCloakService.init({
        clientId: "aurelia-client-id",
        realm: "MyRealm",
        url: "http://localhost:8888/"
      });
    } catch (e) {
      console.debug("Keycloak not initialized")
    }
  }

  async getUnsecured() {
    let response = await this.dataAdapter.getUnsecuredPage();
    this.lastWebRequest = response;
  }

  async getSecured() {
    // if (!this.keyCloakService.isAuthenticated) {
    //   await this.keyCloakService.login();
    // }
    let response = await this.dataAdapter.getSecuredPage();
    this.lastWebRequest = response;
  }

  async logout() {
    await this.keyCloakService.logout();
  }

  async login() {
    await this.keyCloakService.login();
  }
}
