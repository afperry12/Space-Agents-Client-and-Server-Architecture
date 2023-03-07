import React, {useEffect, useState} from 'react';
import { withRouter } from 'react-router';
import Axios from 'axios';

const Profile = () => {
    const [usernameReg, setUsernameReg] = useState("");
    const [passwordReg, setPasswordReg] = useState("");

    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const [loginStatus, setLoginStatus] = useState(false);

    Axios.defaults.withCredentials = true;

    const register = () => {
        Axios.post("http://localhost:3002/register", {
            username: usernameReg,
            password: passwordReg,
        }).then((response) => {
            console.log(response);
        });
    };

    const login = () => {
        Axios.post("http://localhost:3002/login", {
            username: username,
            password: password,
        }).then((response) => {
            if (response.data.error) {
                setLoginStatus(false);
            } else {
                console.log(response.data);
                localStorage.setItem("token", response.data.token);
                setLoginStatus(true);
            }
        });
    };

    // const userAuthenticated = () => {
    //     Axios.get("http://localhost:3002/profile").then((response) => {
    //         console.log(response);
    //     })
    // };

    const userAuthenticated = () => {
        Axios.get("http://localhost:3002/isUserAuth", {
            headers: {
                "x-access-token": localStorage.getItem("token"),
            },
        }).then((response) => {
            console.log(response);
        });
    };

    const logout = () => {
        Axios.post("http://localhost:3002/logout").then((response) => {
            console.log(response);
        })
    };



    return (
        // <div className="container">
        //     <h1 className="text-center" style={{paddingTop: "30%"}}>
        //         Profile
        //     </h1>
        // </div>
        <div className="App">
        Profile
        </div>
    )
}
export default withRouter(Profile);