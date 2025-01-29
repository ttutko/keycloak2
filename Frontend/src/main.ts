import {Aurelia} from 'aurelia-framework';
import environment from '../config/environment.json';
import {PLATFORM} from 'aurelia-pal';
import { ConfigBuilder } from 'aurelia-syncfusion-ej2-bridge';

export function configure(aurelia: Aurelia): void {
  aurelia.use
    .standardConfiguration()
    .feature(PLATFORM.moduleName('resources/index'));

  aurelia.use.developmentLogging(environment.debug ? 'debug' : 'warn');

  if (environment.testing) {
    aurelia.use.plugin(PLATFORM.moduleName('aurelia-testing'));
  }

    aurelia.use.plugin(PLATFORM.moduleName('aurelia-syncfusion-ej2-bridge'), (config: ConfigBuilder) => {
        config.useAll();
    });

  aurelia.start().then(() => aurelia.setRoot(PLATFORM.moduleName('app')));
}
