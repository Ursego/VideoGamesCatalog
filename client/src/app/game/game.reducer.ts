import { createReducer, on } from "@ngrx/store";
import { CrudOperation } from "../util/util";
import { initialGameState } from "./game.state";
import { createBlankGame } from "./game.model";
import {
    SetHighlightedGameAction,
    SwitchToInsertModeAction,
    SwitchToUpdateModeAction,
    CleanUpAfterSaveAction,
    SelectGameListAction,
    SelectGameListSuccessAction,
    SelectGameListFailureAction,
    InsertGameAction,
    InsertGameSuccessAction,
    InsertGameFailureAction,
    UpdateGameAction,
    UpdateGameSuccessAction,
    UpdateGameFailureAction,
    DeleteGameAction,
    DeleteGameSuccessAction,
    DeleteGameFailureAction,
    SelectGameCategoryListAction,
    SelectGameCategoryListSuccessAction,
    SelectGameCategoryListFailureAction,
    SelectAgeRatingListAction,
    SelectAgeRatingListSuccessAction,
    SelectAgeRatingListFailureAction
} from "./game.actions";

export const gameReducer = createReducer(
    initialGameState,

    on(SetHighlightedGameAction, (state, { highlightedGame }) => ({ ...state,
        highlightedGame: { ...highlightedGame }
    })),
    on(SwitchToInsertModeAction, state => ({ ...state, 
        crudOperationToPerform: CrudOperation.Insert, // inform Game Card that it's open for INSERT
        gameToSave: createBlankGame() // prepare the Game to populate and INSERT
    })),
    on(SwitchToUpdateModeAction, (state, { gameToSave: game }) => ({ ...state,
        crudOperationToPerform: CrudOperation.Update, // inform Game Card that it's open for UPDATE
        gameToSave: { ...game } // provide the Game to UPDATE
    })),
    on(CleanUpAfterSaveAction, state => ({ ...state,
        gameToSave: null,
        crudOperationToPerform: null
    })),
        
    // Populate Game List:
    on(SelectGameListAction, state => ({ ...state, 
        gameListLoaded: false
    })),
    on(SelectGameListSuccessAction, (state, { gameList }) => ({ ...state, 
        gameList: [...gameList].sort((a, b) => a.name.localeCompare(b.name)),
        gameListLoaded: true
    })),
    on(SelectGameListFailureAction, state => ({ ...state,
        gameListLoaded: true
    })),
    
    // INSERT Game:
    on(InsertGameAction, state => ({ ...state, 
        gameLoaded: false
    })),
    on(InsertGameSuccessAction, (state, { game }) => ({ ...state, 
        gameToSave: { ...game }, // after INSERT, the screen can remain open, and the Game can be changed and UPDATEd
        gameList: [...state.gameList, game].sort((a, b) => a.name.localeCompare(b.name)), // add it to Game List
        highlightedGame: null, // disable Edit & Delete buttons until user clicks a game
        crudOperationToPerform: CrudOperation.Update, // subseqent Save button clicks should UPDATE
        gameLoaded: true
    })),
    on(InsertGameFailureAction, state => ({ ...state,
        gameLoaded: true
    })),
    
    // UPDATE Game:
    on(UpdateGameAction, state => ({ ...state,
        gameLoaded: false
    })),
    on(UpdateGameSuccessAction, (state, { game }) => ({ ...state,
        gameToSave: { ...game }, // after UPDATE, the screen can remain open, and the Game can be changed and UPDATEd again
        gameList: state.gameList.map(g => (g.id === game.id ? game : g)).sort((a, b) => a.name.localeCompare(b.name)), // update it in Game List
        // Disable Edit & Delete buttons until user clicks a game (sort could re-arrange the list if user updated name, so this game could move):
        highlightedGame: null,
        gameLoaded: true
    })),
    on(UpdateGameFailureAction, state => ({ ...state,
        gameLoaded: true
    })),
   
    // DELETE Game:
    on(DeleteGameAction, state => ({ ...state,
        gameListLoaded: false 
    })),
    on(DeleteGameSuccessAction, (state, { gameId }) => ({ ...state,
        gameList: state.gameList.filter(g => g.id !== gameId), // remove it from Game List
        highlightedGame: null, // disable Edit & Delete buttons until user clicks a game again
        gameListLoaded: true
    })),
    on(DeleteGameFailureAction, state => ({ ...state,
        gameListLoaded: true
    })),
    
    // Populate Game Category dropdown:
    on(SelectGameCategoryListAction, state => ({ ...state, 
        gameCategoryListLoaded: false
    })),
    on(SelectGameCategoryListSuccessAction, (state, { gameCategoryList }) => ({ ...state, 
        gameCategoryList: [...gameCategoryList],
        gameCategoryListLoaded: true
    })),
    on(SelectGameCategoryListFailureAction, state => ({ ...state, 
        gameCategoryListLoaded: true
    })),
    
    // Populate Age Rating dropdown:
    on(SelectAgeRatingListAction, state => ({ ...state, 
        ageRatingListLoaded: false
    })),
    on(SelectAgeRatingListSuccessAction, (state, { ageRatingList }) => ({ ...state, 
        ageRatingList: [...ageRatingList],
        ageRatingListLoaded: true
    })),
    on(SelectAgeRatingListFailureAction, state => ({ ...state, 
        ageRatingListLoaded: true
    }))
);
