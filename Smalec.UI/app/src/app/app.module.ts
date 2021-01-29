import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CookieService } from 'ngx-cookie-service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgxsModule } from '@ngxs/store';
import { TranslateLoader, TranslateModule } from '@ngx-translate/core';
import { TranslateHttpLoader } from '@ngx-translate/http-loader';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LoginComponent } from './views/mainpage/login/login.component';
import { RegisterComponent } from './views/mainpage/register/register.component';
import { ControlsModule } from './controls/controls.module';
import { environment } from 'src/environments/environment';
import { MainpageWrapperComponent } from './views/mainpage/mainpage-wrapper/mainpage-wrapper.component';
import { LayoutState } from './store/layout/layout-state';
import { TitleState, PostState, UsersState, UserDataState } from './store';
import { TokenInterceptor } from './helpers';
import { SearchState } from './store/search/search-state';

@NgModule({
    declarations: [
        AppComponent,
        LoginComponent,
        RegisterComponent,
        MainpageWrapperComponent,
    ],
    imports: [
        BrowserModule,
        BrowserAnimationsModule,
        FormsModule,
        AppRoutingModule,
        HttpClientModule,
        ReactiveFormsModule,
        ControlsModule,
        TranslateModule.forRoot({
            loader: {
                provide: TranslateLoader,
                useFactory: HttpLoaderFactory,
                deps: [HttpClient]
            }
        }),
        NgxsModule.forRoot([
            UserDataState,
            TitleState,
            PostState,
            UsersState,
            LayoutState,
            SearchState
        ],
        {
            developmentMode: !environment.production
        })
    ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: TokenInterceptor,
            multi: true,
        },
        CookieService
    ],
    bootstrap: [AppComponent]
})
export class AppModule { }

export function HttpLoaderFactory(http: HttpClient) {
    return new TranslateHttpLoader(http);
}