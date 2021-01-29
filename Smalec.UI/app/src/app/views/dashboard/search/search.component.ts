import { AfterViewInit, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { fromEvent, Observable } from 'rxjs';
import { debounceTime, distinctUntilChanged, filter, takeUntil, tap } from 'rxjs/operators';
import { User } from 'src/app/models';
import { DestroyableComponent } from 'src/app/shared';
import { Search } from 'src/app/store/search/search-actions';
import { SearchState } from 'src/app/store/search/search-state';

@Component({
  selector: 'app-search',
  templateUrl: './search.component.html',
  styleUrls: [
    './search.component.scss',
    '../../../styles/controls/mobile-menu.scss']
})
export class SearchComponent extends DestroyableComponent implements AfterViewInit, OnInit {

  isSearchLoading = false;
  searchKeywords = '';

  constructor(private store: Store) {
    super();
  }

  @ViewChild('searchInput') searchInput: ElementRef;
  @ViewChild('searchResult') searchResult: ElementRef;

  @Select(SearchState.searchResult) searchResult$: Observable<User[]>;
  searchResultData: User[] = [];

  ngOnInit(): void {
    this.searchResult$.pipe(takeUntil(this.destroy$))
      .subscribe(x => {
        this.isSearchLoading = false;
        this.searchResultData = x;
      })
  }

  ngAfterViewInit(): void {
    fromEvent(this.searchInput.nativeElement, 'keyup')
      .pipe(
        takeUntil(this.destroy$),
        filter(Boolean),
        debounceTime(150),
        distinctUntilChanged(),
        tap(() => {
          if (this.searchKeywords) {
            this.isSearchLoading = true;
            this.showResultContainer();
            this.store.dispatch(new Search(this.searchKeywords));
          } else {
            this.isSearchLoading = false;
            this.hideResultContainer();
          }
        })
      )
      .subscribe();
  }

  onSearchInputClick(): void {
    if (!this.searchKeywords) {
      this.hideResultContainer();
    } else {
      this.showResultContainer();
    }
  }

  showResultContainer(): void {
    this.searchResult.nativeElement.style.display = "block";
  }

  hideResultContainer(): void {
    this.searchResult.nativeElement.style.display = "none";
  }
}
