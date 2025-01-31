import { KeycloakService } from 'keycloackService';
import { autoinject } from 'aurelia-framework';
import { DataAdapter } from 'dataAdapter';
import { RemovingEventArgs, UploaderModel } from '@syncfusion/ej2/inputs';
import { Ej2Uploader, Ej2UploaderDataAdapter } from 'aurelia-syncfusion-ej2-bridge';
import { HttpClient } from 'aurelia-fetch-client';

@autoinject
export class App {
  private lastWebRequest = "";
  files = [];
  widget: Ej2Uploader = null;
  metadata = {
    name: "Joe Smith",
    age: 50
  };

  uploaderModel: UploaderModel = {
    asyncSettings: {
      saveUrl: "https://web.dev.fa.com/upload",
      removeUrl: "http://localhost:5000/removeUpload"
    },
    maxFileSize: 30000000000,
    multiple: false
  }
  _dataAdapter: Ej2UploaderDataAdapter = {
    remove: async (file: any) => {
      await this.httpClient.fetch(this.uploaderModel.asyncSettings.removeUrl + "/" + file.id, { method: "post" })
        .catch((e) => {
          throw "File could not be delete"
        })
        .then((response: Response) => {
          if (!response.ok) {
            throw "File could not be delete"
          }
        });
    }
  }
  constructor(private keyCloakService: KeycloakService, private dataAdapter: DataAdapter, private httpClient: HttpClient) {

  }

  async attached() {
    try {
      await this.keyCloakService.init({
        clientId: "DevFrontend",
        realm: "DevRealm",
        url: "https://keycloak.dev.fa.com/"
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

  onSuccess(args: any) {
    console.log(`success args`, args);
  }
  generateMetaData(file) {
    let metadata: any = {};

    metadata.fileContextId = "blah";

    return metadata;
  }

  onRemoving(args: RemovingEventArgs) {
    // args.cancel = true;
  }

  onRemoved(args: any) {
    console.log('file removed successsfully', args);
  }
}
