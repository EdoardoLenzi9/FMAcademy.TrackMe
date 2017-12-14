import { TrackMePage } from './app.po';

describe('track-me App', () => {
  let page: TrackMePage;

  beforeEach(() => {
    page = new TrackMePage();
  });

  it('should display welcome message', () => {
    page.navigateTo();
    expect(page.getParagraphText()).toEqual('Welcome to app!!');
  });
});
