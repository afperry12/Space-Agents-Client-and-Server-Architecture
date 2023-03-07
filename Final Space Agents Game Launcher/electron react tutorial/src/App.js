import React, { useState, useEffect } from "react";
import '../node_modules/bootstrap/dist/css/bootstrap.min.css';
import Axios from 'axios';
import { BrowserRouter, HashRouter, Route, Router, Switch } from "react-router-dom";


import Header from "./components/Header";
import Home from "./components/LandingPage";
import News from "./components/News";
import Store from "./components/Store";
import Profile from "./components/Profile";
import ProtectedRoute from "./components/ProtectedRoute";
import Login from "./components/Login";

function App () {
  const [isAuth, setIsAuth] = useState(false);

  useEffect(() => {
    Axios.get("http://localhost:3002/isUserAuth", {
        headers: {
            "x-access-token": localStorage.getItem("token"),
        },
    }).then((response) => {
        console.log(response);
        if (response.data.auth==true){
                setIsAuth(true);
                } else{
                  setIsAuth(false);
                }
    }).catch(error=>{
      console.log(error);
    });
}, []);

// const userAuthenticated = () => {
//    Axios.get("http://localhost:3002/isUserAuth", {
//       headers: {
//           "x-access-token": localStorage.getItem("token"),
//       },
//   }).then((response) => {
//       console.log(response);
//       if (response.data.auth==true){
//       setIsAuth(true);
//       } else{
//         setIsAuth(false);
//       }
//   });
// };
// console.log(localStorage.getItem("token"))

  return (
    <BrowserRouter>
        <Header/>
        {/* {userAuthenticated()} */}
            <Switch>
             <Route path="/home" component={Home} exact/>
             <Route path="/news" component={News} exact/>
             <Route path="/store" component={Store} exact/>
             <Route path="/login" component={Login} exact/>
             <ProtectedRoute path="/profile" component={Profile} isAuth={isAuth} exact/>
             <Route path="*" component={Home} exact/>
           </Switch>
      </BrowserRouter>
  )

}

export default App

// const ENDPOINT = "http://localhost:8080";


// function App() {
//   const [data, setData] = useState([]);

// //   var socket = socketIOClient (ENDPOINT);
// //     socket.on('connection', () => {
// //         console.log(`I'm connected with the back-end`);
// // });

//   useEffect(() => {
//     const socket = socketIOClient(ENDPOINT);
//     socket.on('hi', cpuPercent => {
//       console.log(cpuPercent);
//       setData(currentData => [...currentData, cpuPercent]);
//     });
//   }, []);

//   return (
//     <div>
//       <h1>CPU Usage Chart</h1>
//       <LineChart width={500} height={500} data={data}>
//         <XAxis dataKey="name" />
//         <YAxis />
//         <CartesianGrid stroke="#efg43g" strokeDasharray="5 5" />
//         <Line type="monotone" dataKey="value" stroke="#355jgf"/>
//       </LineChart>
//     </div>
//   );
// }

// export default App;