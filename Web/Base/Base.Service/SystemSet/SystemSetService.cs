namespace Base.Service.SystemSet
{
    public static class SystemSetService
    {
        public static CommonService Common { get { return CommonService.Single; } }
        public static EntityService Entity { get { return EntityService.Single; } }
        public static FieldService Field { get { return FieldService.Single; } }
        public static FormService Form { get { return FormService.Single; } }
        public static ListDataSourceService ListDataSource { get { return ListDataSourceService.Single; } }
        public static FormDataSourceService FormDataSource { get { return FormDataSourceService.Single; } }
        public static LookUpService LookUp { get { return LookUpService.Single; } }
        public static SearchService Search { get { return SearchService.Single; } }
        public static ViewService View { get { return ViewService.Single; } }
        public static DictionaryService Dictionary { get { return DictionaryService.Single; } }

    }
}
