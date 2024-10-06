import { HttpClient } from 'aurelia-fetch-client';
import Keycloak from "keycloak-js";
import { computedFrom, autoinject, LogManager } from "aurelia-framework";
import { Logger } from "aurelia-logging";

const logger: Logger = LogManager.getLogger('KeycloakService');

@autoinject
export class KeycloakService {
    public instance: Keycloak = null;
    private silentCheckSso: string = "silent-check-sso.html";
    public profile: Keycloak.KeycloakProfile = null;

    constructor(private httpClient: HttpClient) {

    }

    @computedFrom("instance", "instance.authenticated")
    public get isAuthenticated(): boolean {
        return this.instance?.authenticated;
    }

    public async getToken() {
        logger.debug("getToken");
        await this.instance.updateToken(30);
        logger.debug("getToken complete", this.instance.token);

        return this.instance.token;
    }

    public async login() {
        await this.instance.login();
    }

    public async init(config: KeycloakConfiguration) {
        this.instance = new Keycloak(config);

        let initOptions: Keycloak.KeycloakInitOptions = {
            onLoad: 'check-sso',
            enableLogging: true,
            silentCheckSsoRedirectUri: `${window.location.origin}/${this.silentCheckSso}`
        }

        try {
            let authenticated = await this.instance.init(initOptions);

            if (!authenticated) {
                logger.debug("KeycloakService: Not authenticated at init")

            } else {
                logger.debug("KeycloakService: Authenticated")
                this.profile = await this.instance.loadUserProfile();
                logger.debug("profile", this.profile)
            }
        }
        catch (error) {
            logger.error("Failed to init", error);
        }


        this.instance.onAuthSuccess = () => { }
        this.instance.onAuthLogout = () => { }
        this.instance.onAuthRefreshError = () => { }
        this.instance.onAuthRefreshSuccess = () => { }
        this.instance.onTokenExpired = () => { }
        this.instance.onAuthError = () => { };

        this.configureHttpClient();
    }

    private configureHttpClient() {
        let _this = this;
        this.httpClient.configure(config => {
            config
                .withDefaults({
                    credentials: 'same-origin',
                    headers: {
                        'Accept': 'application/json',
                        'X-Requested-With': 'Fetch'
                    }
                })
                .withInterceptor({
                    async request(request) {
                        console.log(`Requesting ${request.method} ${request.url}`);
                        let token = await _this.getToken();
                        request.headers.append("Authorization", `Bearer ${token}`)
                        console.log("Request interceptor complete");
                        return request;
                    },
                    response(response) {
                        console.log(`Received ${response.status} ${response.url}`);
                        return response;
                    }
                });
        });
    }

    public async logout() {
        return await this.instance.logout();
    }


}

export interface KeycloakConfiguration {
    url: string;
    realm: string;
    clientId: string;
}
