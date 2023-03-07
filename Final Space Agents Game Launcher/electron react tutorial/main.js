
const { BrowserWindow, app, ipcMain, Notification } = require('electron');
const path = require('path');
const { download } = require("electron-dl");
const fs = require('fs');
var Zip = require("adm-zip");
const Store = require('electron-store');
const store = new Store();
var exec = require('child_process').execFile;

const isDev = !app.isPackaged;

let win;
async function createWindow() {
  win = new BrowserWindow({
    width: 1200,
    height: 800,
    backgroundColor: "white",
    webPreferences: {
      nodeIntegration: true,
      enableRemoteModule: true,
      worldSafeExecuteJavaScript: true,
      contextIsolation: true,
      preload: path.join(__dirname, 'preload.js')
    }
  })
  
  win.loadFile('index.html');

  let location = '/';
  win.webContents.on('did-navigate-in-page', () => {
    location = win.webContents.getURL().replace(/^file:\/\//, '');
  });

  win.webContents.on('did-fail-load', () => { console.log('on browser reload it did-fail-load and reloaded the app'); win.loadURL(`file://${__dirname}//index.html`); });
}


if (isDev) {
  require('electron-reload')(__dirname, {
    electron: path.join(__dirname, 'node_modules', '.bin', 'electron')
  })
}

ipcMain.on("gameExists", async (event) => {
  if (store.get("installed")==true&&fs.existsSync(store.get("installDirectory").concat("TheFinalSpaceAgents.exe"))) {
    console.log(store.get("installed"));
      console.log(fs.existsSync(store.get("installDirectory").concat("TheFinalSpaceAgents.exe")));
    
    await win.webContents.send("responseGameDoesExist", true);
    } else {
      console.log(store.get("installed"));
      console.log(fs.existsSync(string.concat(store.get("installDirectory"),"TheFinalSpaceAgents.exe")));
    }
  });

ipcMain.on("download", (event, info) => {
  info.properties.onProgress = status => win.webContents.send("downloadProgress", status);
  download(BrowserWindow.getFocusedWindow(), info.url, info.properties)
      .then(dl => {
        console.log(dl.getFilename())
        win.webContents.send("downloadComplete", dl.getSavePath())});
     
});

ipcMain.on("UnzipMain", async (event, info) => {
  console.log("starting unzip...");
  console.log(info);
  const finalFile = info.substring(info.lastIndexOf("\\")).replace("\\", "");
  console.log(finalFile);
  const finalPathToContents = info.replace(finalFile,"");
  console.log(finalPathToContents);

const zip = new Zip(info);
await zip.extractAllTo(finalPathToContents, false);
fs.unlinkSync(info);
const gameVersion = info.substring(info.lastIndexOf("\\")).replace("\\", "").replace(/[^0-9]/g,'');
      store.set('installed', true);
      store.set('installDirectory', finalPathToContents);
      store.set('fileName', finalFile);
      store.set('gameversion', gameVersion);
      console.log(store.get('gameversion'));
      console.log("made it!")
      win.webContents.send("unzipComplete", info);
});

ipcMain.on("getInstallDirectory", async (event, info) => {
const installDir = store.get("installDirectory");
const fileName = store.get("fileName");
win.webContents.send("receiveInstallDirectory", installDir);
});

ipcMain.on("launchExeMain", async (event, info) => {
  console.log("storedir: "+store.get("installDirectory"))
  console.log("fileName: "+store.get("fileName"))
  const installDir = store.get("installDirectory");
  const fileName = store.get("fileName");
  let promise = new Promise((resolve, reject) => {
    console.log(info);
    exec("TheFinalSpaceAgents.exe", ['--token', `${info}`], { cwd: installDir }, (err, data) => {
        if (err) reject(err);
        else resolve(data);
    });
});
return promise;
  });

ipcMain.on("getFileName", async (event, info) => {
  const fileName = store.get("fileName");
  // win.webContents.send("receiveFileName", fileName);
  });

ipcMain.on('notify', (_, message) => {
  new Notification({title: 'Notifiation', body: message}).show();
});

ipcMain.on("toMain", (event, args) => {
  info.properties.onProgress = status => win.webContents.send("download progress", status);
  download(BrowserWindow.getFocusedWindow(), info.url, info.properties)
      .then(dl => win.webContents.send("download complete", console.log(dl.getSavePath())));
    win.webContents.send("fromMain", "helllooo");
});

app.whenReady().then(createWindow)
