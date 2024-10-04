import { KeycloakService } from 'keycloackService';
import { autoinject } from 'aurelia-framework';

@autoinject
export class App {

  constructor(private keyCloakService: KeycloakService) {

  }

  async attached() {
    try {
      await this.keyCloakService.init({
        clientId: "ASDF",
        realm: "ASDF",
        url: "ASDF"
      });
    } catch (e) {
      console.debug("Keycloak not initialized")
    }
  }

  getUnsecured() {

  }

  async getSecured() {
    if (!this.keyCloakService.isAuthenticated) {
      await this.keyCloakService.login();
    }
  }
}
