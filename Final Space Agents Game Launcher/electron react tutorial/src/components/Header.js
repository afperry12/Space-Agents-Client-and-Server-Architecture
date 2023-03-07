import React, { useEffect } from 'react';
import './Header.css';
import { NavLink } from 'react-router-dom';
import $ from 'jquery';


const Header = () => {
    function animation(){
        var tabsNewAnim = $('#navbarSupportedContent');
        var activeItemNewAnim = tabsNewAnim.find('.active');
        var activeWidthNewAnimHeight = activeItemNewAnim.innerHeight();
        var activeWidthNewAnimWidth = activeItemNewAnim.innerWidth();
        var itemPosNewAnimTop = activeItemNewAnim.position();
        var itemPosNewAnimLeft = activeItemNewAnim.position();
        $(".hori-selector").css({
          "top":itemPosNewAnimTop.top + "px", 
          "left":itemPosNewAnimLeft.left + "px",
          "height": activeWidthNewAnimHeight + "px",
          "width": activeWidthNewAnimWidth + "px"
        });
        $("#navbarSupportedContent").on("click","li",function(e){
          $('#navbarSupportedContent ul li').removeClass("active");
          $(this).addClass('active');
          var activeWidthNewAnimHeight = $(this).innerHeight();
          var activeWidthNewAnimWidth = $(this).innerWidth();
          var itemPosNewAnimTop = $(this).position();
          var itemPosNewAnimLeft = $(this).position();
          $(".hori-selector").css({
            "top":itemPosNewAnimTop.top + "px", 
            "left":itemPosNewAnimLeft.left + "px",
            "height": activeWidthNewAnimHeight + "px",
            "width": activeWidthNewAnimWidth + "px"
          });
        });
      }
    
      useEffect(() => {
        
        animation();
        $(window).on('resize', function(){
          setTimeout(function(){ animation(); }, 500);
        });
        
      }, []);

    return (
        <nav className="navbar navbar-expand-lg navbar-mainbg">
            <button 
        className="navbar-toggler"
        onClick={ function(){
          setTimeout(function(){ animation(); });
        }}
        type="button" data-toggle="collapse" data-target="#navbarSupportedContent" aria-controls="navbarSupportedContent" aria-expanded="false" aria-label="Toggle navigation">
        <i className="fas fa-bars text-white"></i>
      </button>

      <div 
        className="collapse navbar-collapse" id="navbarSupportedContent">
        <ul className="navbar-nav ml-auto nav-fill w-100">
            
            <div className="hori-selector">
              <div className="left"></div>
              <div className="right"></div>
            </div>
            
            <li className="nav-item active">
              <NavLink className="nav-link" to="/home" exact>
                <i 
                className="fas fa-tachometer-alt">
                </i>Home
              </NavLink>
            </li>

            <li className="nav-item">
              <NavLink className="nav-link" to="/news" exact>
                <i 
                className="far fa-address-book">
                </i>News
              </NavLink> 
            </li>

            <ul className="navbar-logo-ui">
            <NavLink className="navbar-logo" to="/" exact>
                Space Agents
            </NavLink>
            </ul>

            <li className="nav-item">
              <NavLink className="nav-link" to="/store" exact>
                <i 
                className="far fa-clone">
                </i>Store
              </NavLink>
            </li>
            <li className="nav-item">
              <NavLink className="nav-link" to="/profile" exact>
                <i 
                className="far fa-copy">
                </i>Profile
              </NavLink>
            </li>
        </ul>
      </div>
        </nav>
    )
}
export default Header;

// import Navbar from 'react-bootstrap/Navbar';
// import Nav from 'react-bootstrap/Nav';
// import NavDropdown from 'react-bootstrap/NavDropdown';

// class Header extends React.Component {

// render() {
//         const appNavbar = (
//             <Navbar collapseOnSelect expand="lg" bg="dark" variant="dark">
//                 <Navbar.Brand href="#home">React-Bootstrap</Navbar.Brand>
//                 <Navbar.Toggle aria-controls="responsive-navbar-nav" />
//                 <Navbar.Collapse id="responsive-navbar-nav">
//                     <Nav className="mr-auto" style={{display:"flex", flexDirection:"row"}}>
//                         <Nav.Link href="#features">Features</Nav.Link>
//                         <Nav.Link href="#pricing">Pricing</Nav.Link>
//                         <NavDropdown title="Dropdown" id="collasible-nav-dropdown">
//                             <NavDropdown.Item href="#action/3.1">Action</NavDropdown.Item>
//                             <NavDropdown.Item href="#action/3.2">Another action</NavDropdown.Item>
//                             <NavDropdown.Item href="#action/3.3">Something</NavDropdown.Item>
//                             <NavDropdown.Divider />
//                             <NavDropdown.Item href="#action/3.4">Separated link</NavDropdown.Item>
//                         </NavDropdown>
//                     </Nav>
//                     <Nav>
//                         <Nav.Link href="#deets">More deets</Nav.Link>
//                         <Nav.Link eventKey={2} href="#memes">Dank memes</Nav.Link>
//                     </Nav>
//                 </Navbar.Collapse>
//             </Navbar>
//     );

//     return (
//     <div>
//       {appNavbar}
//     </div>
//     );
//   }
// }

// export default Header;