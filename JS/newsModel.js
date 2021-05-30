class NewsRecord {
	constructor(title, keywords) {
		this.title = title;
		this.keywords = keywords;
	}

	getRecordType() {
		return 'base';
	}
}

class TextNewsRecord extends NewsRecord {
	constructor(title, keywords, content) {
		super(title, keywords);

		this.content = content;
	}

	getRecordType() {
		return 'text';
	}
}

class VideoNewsRecord extends NewsRecord {
	constructor(title, keywords, url) {
		super(title, keywords);

		this.url = url;
	}

	getRecordType() {
		return 'video';
	}
}

module.exports = {
	TextNewsRecord,
	VideoNewsRecord
}