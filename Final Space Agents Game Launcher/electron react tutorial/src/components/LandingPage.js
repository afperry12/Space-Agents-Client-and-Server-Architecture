import React, { useState, useEffect, Component } from "react";
import Header from './Header';
import socketIOClient from "socket.io-client";
import {
  BarChart,
  Bar,
  Line,
  LineChart,
  XAxis,
  YAxis,
  Tooltip,
  CartesianGrid,
  ResponsiveContainer,
  AreaChart,
  Area,
  Label
} from 'recharts';
import {ProgressBar} from "react-progressbar-fancy";
import './LandingPage.css'
// const socket = io('http://localhost:8080', {
//   transports: ['websocket', 'polling']
// })

const ENDPOINT = "http://localhost:8080";

function LandingPage(){
    const [data, setData] = useState([]);
    const [showProgressBar, setShowProgressBar] = useState("none");
    const [showUnzipping, setShowUnzipping] = useState("none");
    const [progressBarProgress, setProgressBarProgress] = useState(0);
    const [downloadButton, setDownloadButton] = useState("inline");
    const [playButton, setPlayButton] = useState("none");
      var socket = socketIOClient (ENDPOINT);
        socket.on('connection', () => {
            console.log(`I'm connected with the back-end`);
    });

    useEffect(() => {
      async function fetchData() {
      await window.electron.api.checkGameExists("gameExists");
      await window.electron.api.responseGameExists("responseGameDoesExist", async (bool) => {
        console.log(`Install Directory: ${bool}`);
        setDownloadButton("none");
        setPlayButton("inline");
      });
      }
      fetchData();
    }, []);



      useEffect(() => {
        var socket = socketIOClient (ENDPOINT);
        socket.on('totalUsers', (total) => {
          console.log(total);
          setData(currentData => [...currentData, total]);
        });
      }, []);

      useEffect(() => {
        window.electron.api.responseProgress("downloadProgress", (progress) => {
        document.getElementById("btnId").style.display = "none";
        setShowProgressBar("inline");
        setProgressBarProgress(Math.round(((progress.percent*100) + Number.EPSILON) *100)/100);
        console.log(progress);
        console.log(progress.percent);
        });
          }, []);

      useEffect(() => {
    window.electron.api.responseComplete("downloadComplete", async (here) => {
      setShowProgressBar("none");
      setShowUnzipping("inline");
      await window.electron.api.unzipFile("UnzipMain", here);
    });
      }, []);

      useEffect(() => {
        window.electron.api.unzipComplete("unzipComplete", async (here) => {
          setShowUnzipping("none");
          console.log(`Download Completed: ${here} from main process...`);
          setPlayButton("inline");
        });
          }, []);

      // useEffect(() => {
      //   window.electron.api.receiveInstallDirectory("receiveInstallDirectory", async (install) => {
      //       console.log(`Install Directory: ${install}`);
      //       await window.electron.api.launchExe("launchExeMain",install);
      //   });
      //     }, []);


        document.body.style = 'background: #7A00B2;';

        // function createData(time, amount) {
        //   return { time, amount };
        // }
        
        // const data = [
        //   createData("00:00", 0),
        //   createData("03:00", 300),
        //   createData("06:00", 600),
        //   createData("09:00", 800),
        //   createData("12:00", 1500),
        //   createData("15:00", 2000),
        //   createData("18:00", 2400),
        //   createData("21:00", 2400),
        //   createData("24:00", undefined)
        // ];

//         const data = [];

//         const rand = 300;
// for (let i = 0; i < 7; i++) {
//   let d = {
//     year: 2000 + i,
//     value: Math.random() * (rand + 50) + 100
//   };

//   data.push(d);
// }

        return (
            <div className="App-bg">
                <h1 className="main-title">Welcome.</h1>
                {/* <div className="welcome-subtitle">Take me to the tutoring portal</div> */}
                <div>
      <h1>Player Count Chart</h1>
      <LineChart width={500} height={500} data={data}>
        <XAxis dataKey="name" />
        <YAxis />
        <CartesianGrid stroke="#efg43g" strokeDasharray="5 5" />
        <Line type="monotone" dataKey="usercount" stroke="#8884d8"/>
      </LineChart>
      {/* <LineChart
      width={500}
      height={300}
      data={data}
      margin={{ top: 5, right: 20, bottom: 5, left: 0 }}
    >
      <Line type="monotone" dataKey="value" stroke="#8884d8" dot={false} />
      <XAxis dataKey="year" />
      <YAxis />
    </LineChart> */}
      {/* <button onClick={() => {
          window.electron.notificationApi.sendNotification("My custom Noti!");
      }}>Notify</button> */}
    </div>
    <div id="downloadButton" style={{display: downloadButton}}>
    <input type="button" id="btnId" value="Download" onClick={() => {
      window.electron.api.folder("toMain");
    }}/>
    </div>
    <div id="progressBarId" style={{display: showProgressBar}}>
    <ProgressBar label="Downloading..." darkTheme="true" labelDarkTheme="true" primaryColor="#000046" secondaryColor="#1CB5E0" score={progressBarProgress}/>
    </div>
    {/* #40E0D0 */}
    <div id="unzip" style={{display: showUnzipping, color: "#40E0D0"}} >
    <p>Unzipping Files...</p>
    </div>
    <div id="play" style={{display: playButton}}>
    <button onClick={ async () => {
      // await window.electron.api.getInstallDirectory("getInstallDirectory");
      await window.electron.api.launchExe("launchExeMain",localStorage.getItem("token"));
      }}>Play</button>
    </div>
            </div>
        );
        }
        export default LandingPage;