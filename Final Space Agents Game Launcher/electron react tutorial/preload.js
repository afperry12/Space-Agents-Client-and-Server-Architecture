
const { ipcRenderer, contextBridge, } = require('electron');
window.ipcRenderer = require('electron').ipcRenderer;
const {dialog} = require('electron').remote;
contextBridge.exposeInMainWorld('electron', {
  api: {
    checkGameExists: (channel) => {
      let validChannels = ["gameExists"];
      if (validChannels.includes(channel)) {
          ipcRenderer.send("gameExists");
      }
  },
  responseGameExists: (channel, func) => {
    console.log("here!")
    let validChannels = ["responseGameDoesExist"];
    if (validChannels.includes(channel)) { 
        ipcRenderer.on(channel, (event, ...args) => func(...args));
    }
},
    folder: (channel) => {
      const pathArray = dialog.showOpenDialog({defaultPath: 'C://', properties: ['openDirectory']}).then(result => {
        console.log(result.canceled)
        console.log(result.filePaths)
        let validChannels = ["toMain"];
        if (validChannels.includes(channel)&&result.canceled==false) {
            ipcRenderer.send('download', {
        url: "https://s25.filetransfer.io/storage/download/Ex3lvOgJOzNz",
        // url: "https://store1.gofile.io/download/direct/0589f47c-bc14-4636-8007-9b702816bba1/TheFinalSpaceAgents.zip",
        // url: 'https://ucba91fb8bd69a3858c165f89b7d.dl.dropboxusercontent.com/zip_download_get/A2FtfWDWNM_r8tSreFCw-qIvigZAKslqMODykgdnue_CdyhRAFTfkgzWgQ9NAii39cD7jnF5eA8AlUrxZW3OMG98YuSmSJEsrSllg__7E1ZhSA?_download_id=57122936313709649856628079635341171483191454329789792325706741907&_notify_domain=www.dropbox.com&dl=1',
        properties: {
          saveAs: false,
          directory: result.filePaths[0]
        }
      });
        }
      }).catch(err => {
        console.log(err)
      })
    },
    responseProgress: (channel, func) => {
        let validChannels = ["downloadProgress"];
        if (validChannels.includes(channel)) {
            // Deliberately strip event as it includes `sender` 
            ipcRenderer.on(channel, (event, ...args) => func(...args));
        }
    },
    responseComplete: (channel, func) => {
      let validChannels = ["downloadComplete"];
      if (validChannels.includes(channel)) {
          // Deliberately strip event as it includes `sender` 
          ipcRenderer.on(channel, (event, ...args) => func(...args));
      }
  },
    unzipFile: (channel, func) => {
      let validChannels = ["UnzipMain"];
      if (validChannels.includes(channel)) {
          // Deliberately strip event as it includes `sender` 
          ipcRenderer.send("UnzipMain", func);
      }
  },
    unzipComplete: (channel, func) => {
      let validChannels = ["unzipComplete"];
      if (validChannels.includes(channel)) {
          // Deliberately strip event as it includes `sender` 
          ipcRenderer.on(channel, (event, ...args) => func(...args));
      }
  },
    getInstallDirectory: (channel) => {
      let validChannels = ["getInstallDirectory"];
      if (validChannels.includes(channel)) {
        ipcRenderer.send("getInstallDirectory");
      }
    },
    receiveInstallDirectory: (channel, func) => {
      let validChannels = ["receiveInstallDirectory"];
      if (validChannels.includes(channel)) {
          // Deliberately strip event as it includes `sender` 
          ipcRenderer.on(channel, (event, ...args) => func(...args));
      }
  },
  getFileName: (channel) => {
    let validChannels = ["getFileName"];
    if (validChannels.includes(channel)) {
      ipcRenderer.send("getFileName");
    }
  },
//   receiveFileName: (channel, func) => {
//     let validChannels = ["receiveFileName"];
//     if (validChannels.includes(channel)) {
//         // Deliberately strip event as it includes `sender` 
//         ipcRenderer.on(channel, (event, ...args) => func(...args));
//     }
// },
    launchExe: (channel, func) => {
      let validChannels = ["launchExeMain"];
      if (validChannels.includes(channel)) {
          // Deliberately strip event as it includes `sender` 
          ipcRenderer.send("launchExeMain", func);
      }
  },
},
  notificationApi: {
    sendNotification(message) {
      ipcRenderer.send('notify', message);
    }
  },
  // fileDownloaderApi: {
  //   downloadFile(file) {
  //     ipcRenderer.send('download', {
  //       url: 'https://www.learningcontainer.com/wp-content/uploads/2020/05/sample-zip-file.zip',
  //       properties: {
  //         saveAs: true,
  //         directory: 'C:\\'
  //       }
        
  //     });
  //     ipcRenderer.on("download progress", (event, progress) => {
  //       console.log(progress); // Progress in fraction, between 0 and 1
        
  //       const progressInPercentage = progress * 100; // With decimal point and a bunch of numbers
  //       const cleanProgressInPercentages = Math.floor(progress * 100); // Without decimal point
  //     });
  //   }
    
  // },
  batteryApi: {

  },
  filesApi: {

  }
})
