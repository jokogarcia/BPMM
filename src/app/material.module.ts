import {NgModule} from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
const modules =[MatButtonModule,
    MatInputModule,
    MatFormFieldModule];
    
@NgModule({
    imports:[...modules],
    exports:[...modules]
})
export class MaterialModule{};
