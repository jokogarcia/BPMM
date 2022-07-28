if ng build --configuration=production ; then
   echo Build Succesfull. Publishing:
   ./publish.sh
else
   echo Build Failed 
fi
