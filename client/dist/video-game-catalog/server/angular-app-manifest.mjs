
export default {
  bootstrap: () => import('./main.server.mjs').then(m => m.default),
  inlineCriticalCss: true,
  baseHref: '/',
  locale: undefined,
  routes: [
  {
    "renderMode": 2,
    "route": "/"
  },
  {
    "renderMode": 2,
    "preload": [
      "chunk-RUAM5G2Y.js"
    ],
    "route": "/games"
  },
  {
    "renderMode": 2,
    "redirectTo": "/",
    "route": "/**"
  }
],
  entryPointToBrowserMapping: undefined,
  assets: {
    'index.csr.html': {size: 492, hash: '2eeba5f77d655d19e4bd4c5fc41bc6db15c8fe8ae815b2fa309c1f179cdb6212', text: () => import('./assets-chunks/index_csr_html.mjs').then(m => m.default)},
    'index.server.html': {size: 1005, hash: '588573ba854a8423317b7c8d3c925740faacbe78a658236d88021ecd3378a04b', text: () => import('./assets-chunks/index_server_html.mjs').then(m => m.default)},
    'index.html': {size: 2369, hash: '8e6c507fc13884ccb306684710151cea3a3498b5c557940576aed9ca8209d0cf', text: () => import('./assets-chunks/index_html.mjs').then(m => m.default)},
    'games/index.html': {size: 1003, hash: '82446546f2f48167a562e3a072c875a3caa8e4afc6784e5fea03ed8c5faa46c4', text: () => import('./assets-chunks/games_index_html.mjs').then(m => m.default)},
    'styles-5INURTSO.css': {size: 0, hash: 'menYUTfbRu8', text: () => import('./assets-chunks/styles-5INURTSO_css.mjs').then(m => m.default)}
  },
};
