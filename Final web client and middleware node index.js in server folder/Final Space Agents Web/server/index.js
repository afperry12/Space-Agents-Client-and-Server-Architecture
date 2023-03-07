const express = require("express");
const app = express();
const http = require("http").createServer(app);
const PORT = 8080;
const os = require("os-utils");
const io = require("socket.io")(http, {
    cors: {
      origin: "*"
    }
  });
const db = require("./models");
const { spaceagentsaccountsses } = require("./models");
const { createTokens, validateToken } = require("./JWT");
const STATIC_CHANNELS = ['global_notifications', 'global_chat'];
const cors = require("cors");

const cookieParser = require("cookie-parser");
const session = require("express-session");

const bcrypt = require("bcrypt");
const jwt = require("jsonwebtoken");

const { Pool, Client } = require('pg');

var pgp = require('pg-promise')(/* options */)
var postgre = pgp('postgresql://postgres:57vWHmVEm5fBZZwM@localhost/postgres')

var integer = 0;

var corsOptions = {
  origin: 'http://localhost:3000',
  optionsSuccessStatus: 200, // some legacy browsers (IE11, various SmartTVs) choke on 204
  methods: ["GET", "POST"],
  credentials: true
}

app.use(express.json());
app.use(cors(corsOptions));
app.use(cookieParser());
// app.use(session({
//   key: "userId",
//   secret: "subscribe",
//   resave: false,
//   saveUninitialized: false,
//   cookie: {
//     expires: 60 * 60 * 24,
//   },
// }))

// const pool = new Pool({
//   user: 'postgre',
//   host: 'localhost',
//   database: 'postgres',
//   password: '57vWHmVEm5fBZZwM',
//   port: 5432,
// })


app.post("/register", (req, res) => {
  const { username, password } = req.body;

  bcrypt.hash(password, 10).then((hash) => {
    spaceagentsaccountsses.create({
      username: username,
      password: hash,
    })
      .then(() => {
        res.json("USER REGISTERED");
      })
      .catch((err) => {
        if (err) {
          res.status(400).json({ error: err });
        }
      });
  });
});

app.post("/login", async (req, res) => {
  const { username, password } = req.body;

  const user = await spaceagentsaccountsses.findOne({ where: { username: username } });

  if (!user) res.status(400).json({ error: "User Doesn't Exist" });

  const dbPassword = user.password;
  bcrypt.compare(password, dbPassword).then((match) => {
    if (!match) {
      res
        .status(400)
        .json({ error: "Wrong Username and Password Combination!" });
    } else {
      const accessToken = createTokens(user);

      // res.cookie("access-token", accessToken, {
      //   maxAge: 60 * 60 * 24 * 30 * 1000,
      //   httpOnly: true,
      //   secure: true,
      // });

      res.json(accessToken);
    }
  });
});

app.post("/logout", validateToken, async (req, res) => {
  const accessToken = "denied";

  res.cookie("access-token", accessToken, {
    maxAge: 60 * 60 * 24 * 30 * 1000,
    httpOnly: true,
    secure: true,
  });
});

app.get("/profile", validateToken, (req, res) => {
  res.json("profile");
});

const verifyJWT = (req, res, next) => {
  const token = req.headers["x-access-token"]
  console.log(token);
  if (!token) {
    res.send("no token");
  } else {
    jwt.verify(token, "jwtSecret", (err, decoded) => {
      if (err) {
        res.json({auth: false, message: "Auth Failed"})
      } else {
        req.userId = decoded.id;
        // postgre.one("SELECT username FROM spaceagentsaccountsses WHERE id = '" + postgre.esca 123)
  postgre.one("SELECT username FROM spaceagentsaccountsses WHERE id = $1", req.userId)
  .then( user => {
    console.log(user); // print user object;
    res.json({auth: true, message: "Auth Succeeded", user: user.username});
})
  .catch(function (error) {
    console.log('ERROR:', error)
  })
        next();
      }
    })
  }
}

app.get("/isUserAuth", verifyJWT, (req, res) => {
  
});

db.sequelize.sync().then(() => {
  app.listen(3002, () => {
    console.log("SERVER RUNNING ON PORT 3002");
  });
});

// app.post("/upload", async (req, res) => {
//   try {
//     await uploadFile(req, res);

//     if (req.file == undefined) {
//       return res.status(400).send({ message: "Please upload a file!" });
//     }

//     res.status(200).send({
//       message: "Uploaded the file successfully: " + req.file.originalname,
//     });
//   } catch (err) {
//     console.log(err);

//     if (err.code == "LIMIT_FILE_SIZE") {
//       return res.status(500).send({
//         message: "File size cannot be larger than 2MB!",
//       });
//     }

//     res.status(500).send({
//       message: `Could not upload the file: ${req.file.originalname}. ${err}`,
//     });
//   }
// });

// const verifyJWT = (req, res, next) => {
//   const token = req.headers["x-access-token"]

//   if (!token) {
//     res.send("no token");
//   } else {
//     jwt.verify(token, "jwtSecret", (err, decoded) => {
//       if (err) {
//         res.json({auth: false, message: "Auth Failed"})
//       } else {
//         req.userId = decoded.id;
//         next();
//       }
//     })
//   }
// }

// app.get("/isUserAuth", verifyJWT, (req, res) => {
//   res.send("Yes Authenticated");
// });

// app.get("/login", (req, res) => {
//   if (req.session.user) {
//     res.send({loggedIn: true, user: req.session.user});
//   } else {
//     res.send({ loggedIn: false });
//   }
// });

// app.post("/login", (req, res) => {
//   const username = req.body.username;
//   const password = req.body.password;

//   pool.query(
//     "SELECT * FROM spaceagentsaccounts WHERE username = ?;",
//     username,
//     (err, result) => {
//       if (err) {
//         res.send({ err: err });
//       }
//       if (result.length > 0) {
//         bcrypt.compare(password, result[0].password, (error, response) => {
//           if (response) {
//             const id = result[0].id
//             const token = jwt.sign({id}, "jwtSecret", {
//               expiresIn: 300,
//             })
//             req.session.user = result;

//             res.json({auth: true, token: token, result: result})
//           } else {
//             res.send({ message: "Wrong usernname/password!" })
//           }
//         });
//       } else {
//         res.send({ message: "User does not exist!" });
//       }
//     }
//   );
// });

// app.listen(3002, () => {
//   console.log("running server");
// });

// io.on("connection", (socket) => {
//     console.log("New client connected");
//     if (interval) {
//       clearInterval(interval);
//     }
//     interval = setInterval(() => getApiAndEmit(socket), 1000);
//     socket.on("disconnect", () => {
//       console.log("Client disconnected");
//       clearInterval(interval);
//     });
//   });

io.on('connection', (socket) => { /* socket object may be used to send specific messages to the new connected client */
    console.log('CONNECTED: ' + socket.remoteAddress + ':' + socket.remotePort);

    // setInterval(() => {
    //     os.cpuUsage(cpuPercent => {
    //         socket.emit('cpu', {
    //             name: 'cpu',
    //             value: cpuPercent
    //         });
    //     });
    // }, 1000);

    // io.on("hi", (gameSocket) => {
    //     console.log("Someone has said hi!");
    //     os.cpuUsage(cpuPercent => {
    //     socket.emit('cpu', {
    //         name: 'cpu',
    //         value: cpuPercent
    //     });
    // });
    //     gameSocket.emit("hi", `hi Peter, You are connected to the server`);
    // });

    socket.on("hi", name => {
      let date_ob = new Date();
      let date = ("0" + date_ob.getDate()).slice(-2);
      let hours = date_ob.getHours();
      let minutes = date_ob.getMinutes();
      let seconds = date_ob.getSeconds();
        io.emit("hi", `hi ${name}, You are connected to the server`);
        integer = integer + 1;
        console.log(integer);
        io.emit("totalUsers", {
          time: hours+minutes+seconds,
          usercount: `${integer}`
        });
    });

    socket.on("bye", name => {
      // io.emit("hi", `hi ${name}, You are connected to the server`);
      integer = integer - 1;
      console.log(integer);
      io.emit("totalUsers", `${integer}`);
  });

    socket.on('disconnect', () => {
      console.log("A socket Disconnected ..");
      });

    io.on('disconnect', () => {
        console.log("An io Disconnected ..");
        });
    // socket.emit('connection', null);
});



io.on("change", (val1, val2) => {
    socket.emit("change", val2, val1);
});

http.listen(PORT, () => {
  console.log(`server running at port ${PORT}`);
});