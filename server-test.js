var net = require('net');

var client = new net.Socket();
client.connect(8787, 'beer.sofaman.cc', function() {
  console.log('Connected');
  var message = new Buffer([0xa5, 0x01, 0x20, 0x04, 0x00, 0x00, 0x01, 0x00, 0x02]);
  client.write(message);
});

client.on('data', function(data) {
  console.log('Received: ' + data);
  console.log(data.map(i => i));
	client.destroy();
});

client.on('close', function() {
	console.log('Connection closed');
});
