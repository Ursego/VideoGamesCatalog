import { createFeatureSelector, createSelector } from "@ngrx/store";
import { IGameState } from "./game.state";

export const state = createFeatureSelector<IGameState>("game"); // matches in app.config.ts: provideState("game", gameReducer)

export const selectHighlightedGame = createSelector(state, s => s.highlightedGame);

export const selectGameList = createSelector(state, s => s.gameList);
export const selectGameListLoaded = createSelector(state, s => s.gameListLoaded);

export const selectGameToSave = createSelector(state, s => s.gameToSave);
export const selectCrudOperationToPerform = createSelector(state, s => s.crudOperationToPerform);
export const selectGameLoaded = createSelector(state, s => s.gameLoaded);

export const selectGameCategoryList = createSelector(state, s => s.gameCategoryList);
export const selectGameCategoryListLoaded = createSelector(state, s => s.gameCategoryListLoaded);

export const selectAgeRatingList = createSelector(state, s => s.ageRatingList);
export const selectAgeRatingListLoaded = createSelector(state, s => s.ageRatingListLoaded);
