import React from 'react';
import { Route, Redirect } from 'react-router-dom';
import Profile from './Profile';

function ProtectedRoute({component: Component, isAuth, ...rest}) {
    return (
        // <Route {...rest} render={(props)=>{
        //     if (isAuth) {
        //     return <Profile/>;
        //     } else {
        //     return (
        //     <Redirect to={{pathname: '/login', state: {from: props.location}}}/>
        //     );
        //     }
        // }}
        //     />
        <Route
        {...rest}
        render={(props) => isAuth === true
          ? <Component {...props} />
          : <Redirect to={{pathname: '/login', state: {from: props.location}}} />}
      />
    )
}

export default ProtectedRoute;