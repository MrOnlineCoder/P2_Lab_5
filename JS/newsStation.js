const {EventEmitter} = require('events');

module.exports = class NewsStation {
	constructor() {
		this.textNews = [];
		this.videoNews = [];

		this.subscribers = [];
	}

	publish(record) {
		const targetCollection = record.getRecordType() == 'video' ? this.videoNews : this.textNews;

		targetCollection.push(record);

		this.emit(record);
	}

	emit(record) {
		const targetSubscribers = this.subscribers.filter(subscriber => {
			if (subscriber.type != record.getRecordType()) return false;

			if (subscriber.keyword && !record.keywords.includes(subscriber.keyword)) return false;

			return true;
		});

		for (const subscriber of targetSubscribers) {
			// usage of '.call()' / '.apply()' is omitted for the sake of simplicity
			subscriber.callback(record);
		}
	}

	subscribeToTextNews(keyword, callback) {
		this.subscribers.push({
			callback,
			type: 'text',
			keyword: keyword || null
		});
	}

	subscribeToVideoNews(callback) {
		this.subscribers.push({
			callback,
			type: 'video',
			keyword: null
		});
	}
};