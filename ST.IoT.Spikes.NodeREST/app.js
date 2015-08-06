var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var http = require('http');
var routes = require('./routes');

var favicon = require('serve-favicon');
var logger = require('morgan');
var methodOverride = require('method-override');
var session = require('express-session');
var bodyParser = require('body-parser');
var multer = require('multer');
var errorHandler = require('errorhandler');

var router = express.Router();
    router.get('/', function(req, res) {
    console.log(('yeah!'));
});

var app = express();

app.use(bodyParser.json());
app.use(bodyParser.urlencoded({ extended: true }));

app.set('port', process.env.PORT || 8001);
app.use('/api', router);

var server = http.createServer(app);
server.listen(8001, function() {
    console.log("Magic should be happening");
});

module.exports = app;
