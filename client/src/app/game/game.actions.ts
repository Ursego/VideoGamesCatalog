import { createAction, props } from "@ngrx/store";
import { IDropdownEntry } from "../util/util";
import { IGameDto } from "./game.model";

const m = "[Game] " // module
const s = "Success"
const f = "Failure"

let t: String // task

export const SetHighlightedGameAction = createAction("${m}SetHighlightedGame", props<{ highlightedGame: IGameDto }>());
export const SwitchToInsertModeAction = createAction(`${m}SwitchToInsertMode`);
export const SwitchToUpdateModeAction = createAction(`${m}SwitchToUpdateMode`, props<{ gameToSave: IGameDto }>());
export const CleanUpAfterSaveAction = createAction(`${m}CleanUpAfterSave`);

t = `${m}SelectGameList`
export const SelectGameListAction = createAction(`${t}`, props<{ gameName: string }>());
export const SelectGameListSuccessAction = createAction(`${t}${s}`, props<{ gameList: IGameDto[] }>());
export const SelectGameListFailureAction = createAction(`${t}${f}`, props<{ error: unknown }>());

t = `${m}InsertGame`
export const InsertGameAction = createAction(`${t}`, props<{ game: IGameDto }>());
export const InsertGameSuccessAction = createAction(`${t}${s}`, props<{ game: IGameDto }>());
export const InsertGameFailureAction = createAction(`${t}${f}`, props<{ error: unknown }>());

t = `${m}UpdateGame`
export const UpdateGameAction = createAction(`${t}`, props<{ game: IGameDto }>());
export const UpdateGameSuccessAction = createAction(`${t}${s}`, props<{ game: IGameDto }>());
export const UpdateGameFailureAction = createAction(`${t}${f}`, props<{ error: unknown }>());

t = `${m}DeleteGame`
export const DeleteGameAction = createAction(`${t}`, props<{ gameId: number }>());
export const DeleteGameSuccessAction = createAction(`${t}${s}`, props<{ gameId: number }>());
export const DeleteGameFailureAction = createAction(`${t}${f}`, props<{ error: unknown }>());

t = `${m}SelectGameCategoryList`
export const SelectGameCategoryListAction = createAction(`${t}`);
export const SelectGameCategoryListSuccessAction = createAction(`${t}${s}`, props<{ gameCategoryList: IDropdownEntry[] }>());
export const SelectGameCategoryListFailureAction = createAction(`${t}${f}`, props<{ error: unknown }>());

t = `${m}SelectAgeRatingList`
export const SelectAgeRatingListAction = createAction(`${t}`);
export const SelectAgeRatingListSuccessAction = createAction(`${t}${s}`, props<{ ageRatingList: IDropdownEntry[] }>());
export const SelectAgeRatingListFailureAction = createAction(`${t}${f}`, props<{ error: unknown }>());