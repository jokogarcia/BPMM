import {NgModule} from '@angular/core';
import {MatButtonModule} from '@angular/material/button';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatAutocompleteModule } from '@angular/material/autocomplete';
import { MatTableModule } from '@angular/material/table';
import {MatCardModule} from '@angular/material/card';



import {MatPaginatorModule} from '@angular/material/paginator'; 
const modules =[MatButtonModule,
    MatInputModule,
    MatFormFieldModule,
    MatAutocompleteModule,
    MatTableModule,
    MatPaginatorModule,
    MatCardModule
];

@NgModule({
    imports:[...modules],
    exports:[...modules]
})
export class MaterialModule{};
