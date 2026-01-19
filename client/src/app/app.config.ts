import { ApplicationConfig, provideBrowserGlobalErrorListeners, isDevMode } from "@angular/core";
import { provideRouter } from "@angular/router";
import { provideClientHydration, withEventReplay } from "@angular/platform-browser";
import { provideHttpClient } from "@angular/common/http";
import { provideStore, provideState } from "@ngrx/store";
import { provideEffects } from "@ngrx/effects";
import { provideStoreDevtools } from "@ngrx/store-devtools";
import { routes } from "./app.routes";
import { gameReducer } from "./game/game.reducer";
import { GameEffects } from "./game/game.effects";

export const appConfig: ApplicationConfig = {
	providers: [
		provideBrowserGlobalErrorListeners(),
		provideRouter(routes),
		provideClientHydration(withEventReplay()),
		provideHttpClient(),
		provideStore(),
		provideState("game", gameReducer), // matches in selectors: export const state = createFeatureSelector<IGameState>("game");
		provideEffects(GameEffects),
		provideStoreDevtools({ maxAge: 25, logOnly: !isDevMode() })
	]
};
