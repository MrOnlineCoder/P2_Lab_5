const {
	TextNewsRecord,
	VideoNewsRecord
} = require('./newsModel.js');

const NewsStation = require('./newsStation');

const station = new NewsStation();

station.subscribeToTextNews(null, (record) => {
	console.log(`New general text news: ${record.title}`)
});

station.subscribeToTextNews('sport', (record) => {
	console.log(`New sport text news: ${record.title}`)
});

station.subscribeToVideoNews((record) => {
	console.log(`New video news: ${record.title}`)
});

station.publish(
	new TextNewsRecord('British scientists have discovered cure for cancer', ['medicine', 'healthcare'], 'Lorem ipsum')
);

station.publish(
	new VideoNewsRecord('Real life dragons have been found in Carpathian mountains', ['history', 'shocknews', 'discovery'], 'https://www.youtube.com/watch?v=dQw4w9WgXcQ')
);

station.publish(
	new TextNewsRecord('Leage of Champions 2021 is over now. The winner is...', ['sport', 'football'], 'Lorem ipsum')
);